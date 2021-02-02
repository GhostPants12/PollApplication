using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PollApplication.Models;

namespace PollApplication.Controllers
{
    public class PollsController : Controller
    {
        private ApplicationContext _context;
        private int userId;
        public PollsController(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var email = httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Value.Contains("@")).Value;
            userId = _context.Users.FirstOrDefault((user =>
                user.Email == email)).Id;
        }

        public async Task<IActionResult> Index(string searchString, int user)
        {
            userId = user;
            var polls = from p in _context.Polls
                select p;
            polls = polls.OrderByDescending((poll => poll.Date));

            if (!string.IsNullOrEmpty(searchString))
            {
                polls = polls.Where((poll => poll.Name.Contains(searchString)));
            }

            PollViewModel pvm = new PollViewModel()
            {
                Polls = await polls.ToListAsync(),
            };

            return View(pvm);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string searchString)
        {
            var polls = from p in _context.Polls select p;
            if (!string.IsNullOrEmpty(searchString))
            {
                polls = polls.Where((poll => poll.Name.Contains(searchString)));
            }

            PollViewModel pvm = new PollViewModel()
            {
                Polls = await polls.ToListAsync(),
            };

            return View(pvm);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteById(int id)
        {
            foreach (var q in _context.Questions.Where((question => question.LinkedPollId==id)))
            {
                foreach (var answer in _context.Answers.Where((answer => answer.QuestionId==q.Id)))
                {
                    _context.Answers.Remove(answer);
                }

                foreach (var variant in _context.Variants.Where((varia => varia.QuestionId==q.Id)))
                {
                    _context.Variants.Remove(variant);
                }

                _context.Questions.Remove(q);
            }

            _context.Polls.RemoveRange(await _context.Polls.FirstOrDefaultAsync((poll => poll.Id==id)));
            await _context.SaveChangesAsync();

            return RedirectToAction("Delete");
        }

        public async Task<IActionResult> Pass(int id, bool redirect = false)
        {
            Poll p = await _context.Polls.FirstOrDefaultAsync(poll => poll.Id == id);
            if (p is null)
            {
                return RedirectToAction("Index");
            }

            List<Question> questions = _context.Questions.Where((question => question.LinkedPollId == p.Id)).ToList();
            if (!redirect)
            {
                foreach (var q in questions)
                {
                    var foundAnswer = await _context.Answers.FirstOrDefaultAsync((a =>
                        a.UserId == userId && a.QuestionId == q.Id));
                    if (foundAnswer != null)
                    {
                        ViewBag.UserId = userId;
                        ViewBag.PollId = id;
                        return View("Modal");
                    }
                }
            }

            ViewBag.PollName = p.Name;
            if (p.Type == PollType.Check)
            {
                return PassCheck(questions);
            }

            return PassRadio(questions);
        }

        public async Task<IActionResult> Stat(int id)
        {
            Poll p = await _context.Polls.FirstOrDefaultAsync(poll => poll.Id == id);
            if (p is null)
            {
                return RedirectToAction("Index");
            }

            List<Question> questions = await _context.Questions.Where((question => question.LinkedPollId == p.Id)).ToListAsync();
            List<StatViewModel> statView = new List<StatViewModel>();
            int counter = 1;
            foreach (var q in questions)
            {
                List<SimpleReportViewModel> answers = new List<SimpleReportViewModel>();
                foreach (var variant in _context.Variants.Where((variant => variant.QuestionId==q.Id)))
                {
                    SimpleReportViewModel report = new SimpleReportViewModel()
                    {
                        DimensionOne = variant.Text,
                        Quantity = await _context.Answers.Where((answer => answer.VariantId==variant.Id)).CountAsync()
                    };

                    answers.Add(report);
                }

                statView.Add(new StatViewModel(){Models = answers, QuestionText = "Вопрос "+counter+". "+q.QuestionText, Id = counter});
                counter++;
            }

            return View(statView);
        }

        public IActionResult PassRadio(List<Question> questions)
        {
            List<QuestionRadioViewModel> viewList = new List<QuestionRadioViewModel>();
            foreach (Question q in questions)
            {
                QuestionRadioViewModel model = new QuestionRadioViewModel()
                {
                    QuestionId = q.Id,
                    Question = q.QuestionText,
                    Variants = _context.Variants.Where((variant => variant.QuestionId == q.Id))
                        .ToList(),
                };
                viewList.Add(model);
            }


            return View("PassRadio", viewList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PassRadio(List<QuestionRadioViewModel> models)
        {
            if (userId == 1)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                foreach (var model in models)
                {
                    if (model.AnswerId == 0)
                    {
                        ModelState.AddModelError("", "Некорректные ответы");
                        return View(models);
                    }

                    var foundAnswer = await _context.Answers.FirstOrDefaultAsync((a =>
                        a.UserId == userId && a.QuestionId == model.QuestionId));
                    if (foundAnswer != null)
                    {
                        _context.Answers.Remove(foundAnswer);
                    }

                    PollAnswer answer = new PollAnswer()
                    {
                        UserId = userId, QuestionId = model.QuestionId, VariantId = model.AnswerId
                    };

                    _context.Answers.Add(answer);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Некорректные ответы");
            return View(models);
        }

        public IActionResult PassCheck(List<Question> questions)
        {
            List<QuestionCheckViewModel> viewList = new List<QuestionCheckViewModel>();
            foreach (Question q in questions)
            {
                QuestionCheckViewModel model = new QuestionCheckViewModel()
                {
                    QuestionId = q.Id,
                    Question = q.QuestionText,
                    Variants = _context.Variants.Where((variant => variant.QuestionId == q.Id))
                        .ToList(),
                };
                viewList.Add(model);
            }

            return View("PassCheck", viewList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PassCheck(List<QuestionCheckViewModel> models)
        {
            if (userId == 1)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                foreach (var model in models)
                {
                    if (model.Answers.Count==0)
                    {
                        ModelState.AddModelError("", "Некорректные ответы");
                        return View(models);
                    }

                    var foundAnswers = _context.Answers.Where((a =>
                        a.UserId == userId && a.QuestionId == model.QuestionId));
                    if (foundAnswers.Count() != 0)
                    {
                        foreach (var answerFound in foundAnswers)
                        {
                            _context.Answers.Remove(answerFound);
                        }
                    }

                    for (int i = 0; i < model.Answers.Count; i++)
                    {
                        PollAnswer answer = new PollAnswer()
                        {
                            UserId = userId,
                            QuestionId = model.QuestionId,
                            VariantId = model.Variants[model.Answers[i]].Id
                        };

                        _context.Answers.Add(answer);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Некорректные ответы");
            return View(models);
        }
    }
}

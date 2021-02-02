using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using PollApplication.Models;
using PollApplication.Repository;

namespace PollApplication.Controllers
{
    public class PollCreationController : Controller
    {

        private UnitOfWork unitOfWork;
        public PollCreationController(ApplicationContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Poll()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Questions(int count, string name, PollType type, bool isIncorrect)
        {
            ViewBag.Count = count;
            ViewBag.Name = name;
            ViewBag.Type = type;
            ViewBag.IsIncorrect = isIncorrect;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Poll(PollModel model)
        {
            if (ModelState.IsValid)
            {
                Poll poll = unitOfWork.Polls.GetBookList().FirstOrDefault(p => p.Name == model.Name);
                if (poll == null)
                {
                    return RedirectToAction($"Questions", new {model.Count, model.Name, model.Type});
                }

                ModelState.AddModelError("", "Некорректные данные.");
            }

            ModelState.AddModelError("", "Некорректные данные.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Questions(List<QuestionModel> model)
        {
            if (ModelState.IsValid)
            {
                int questionIndex = 1;
                Poll pollToAdd = new Poll() {Name = model.First().Name, Date = DateTime.Now, Type = model.First().Type, Count = model.First().Count};
                unitOfWork.Polls.Create(pollToAdd);
                foreach (var q in model)
                {
                    var questionToAdd = new Question()
                        {LinkedPoll = pollToAdd, QuestionIndex = questionIndex, QuestionText = q.Text};
                    unitOfWork.Questions.Create(questionToAdd);
                    unitOfWork.Save();
                    int variantIndex = 1;
                    foreach (var variant in q.Variants)
                    {
                        unitOfWork.Variants.Create(new Variant() {Question = questionToAdd, Text = variant, VariantIndex = variantIndex++});
                    }

                    unitOfWork.Save();
                    questionIndex++;
                }

                return RedirectToAction("Index", "Home");
            }

            return Questions(model.Count, model.First().Name, model.First().Type, true);
        }
    }
}

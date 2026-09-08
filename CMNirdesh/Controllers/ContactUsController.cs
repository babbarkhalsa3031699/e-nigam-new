using CMNirdesh.Models;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult ContactUs()
        {
            var vm = new ContactUsList();
            var dataModel = new LGContactUsDataViewModel();
            var content = dataModel.GetContent("ContactUs");
            vm.ContactUs = content ?? new ContactUs { PageName = "ContactUs", HtmlContent = "" };
            return View(vm);
        }

        [HttpPost]
        public JsonResult SaveContactUsAjax(string PageName, string HtmlContent)
        {
            var contactUs = new ContactUs
            {
                PageName = PageName,
                HtmlContent = HtmlContent
            };
            var dataModel = new LGContactUsDataViewModel();
            dataModel.SaveOrUpdateContent(contactUs);
            return Json(new { success = true });
        }


    }
}
using ACLager.Controllers;
using ACLager.CustomClasses;

namespace ACLager.ViewModels {
    public class BaseViewModel {
        public bool RenderNavBar { get; set; } = true;
        public string SectionColor { get; set; } = "teal";
        public string SectionName { get; set; } = "";
   
        public void SelectSectionSpecials(string controllername) {
            HomeController hc = new HomeController();
            SectionColor = "teal";
            SectionName = "";
            foreach (HomeMenuBlock hmb in hc.homeMenuBlocks) {
                if (hmb.Controller == controllername) {
                    SectionColor = hmb.Color;
                    SectionName = hmb.Title;
                }
            }
            
        }
    }
}
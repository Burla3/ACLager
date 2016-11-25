using ACLager.Controllers;
using ACLager.CustomClasses;

namespace ACLager.ViewModels {
    public class BaseViewModel {
        public bool RenderNavBar { get; set; } = true;
        public string SectionColor { get; set; } = "teal";
   
        public void SelectColor(string controllername) {
            HomeController hc = new HomeController();
            SectionColor = "teal";
            foreach (HomeMenuBlock hmb in hc.homeMenuBlocks) {
                if (hmb.Controller == controllername) {
                    SectionColor = hmb.Color;
                }
            }
            
        }
    }
}
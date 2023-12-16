namespace Scripts.UI.Scene_UI
{
    public class Intro_UI : UI_Base
    {
        private enum Buttons
        {
            StartBtn,
            ContinueBtn,
            OptionBtn,
            ExitBtn
        }

        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            
            SetButton(typeof(Buttons)); 

            return true;
        }
    }
}
namespace Scripts.UI.Popup
{
    public class Popup : UI_Base
    {
        protected override bool Initialized()
        {
            if (!base.Initialized()) return false;
            Main.UI.OrderLayerToCanvas(gameObject);
            return true;
        }
    }
}
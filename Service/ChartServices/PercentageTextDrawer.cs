using System.Drawing;

namespace RareServer.Service.ChartServices
{
    public interface IPercentageTextDrawer
    {
        void DrawPercentageText(Graphics graphics, string percentageText, float x, float y);
    }

    public class PercentageTextDrawer : IPercentageTextDrawer
    {
        public void DrawPercentageText(Graphics graphics, string percentageText, float x, float y)
        {
            try
            {
                using var font = new Font("Arial", 10, FontStyle.Bold);
                using var textBrush = new SolidBrush(Color.Black);
                graphics.DrawString(percentageText, font, textBrush, x, y);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to draw the percentage.", ex);
            }
        }
    }
}
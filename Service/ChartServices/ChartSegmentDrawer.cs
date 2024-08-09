using System.Drawing;

namespace RareServer.Service.ChartServices
{
    public interface IChartSegmentDrawer
    {
        void DrawSegment(Graphics graphics, Rectangle area, float startAngle, float sweepAngle, Color color);
    }

    public class ChartSegmentDrawer : IChartSegmentDrawer
    {
        public void DrawSegment(Graphics graphics, Rectangle area, float startAngle, float sweepAngle, Color color)
        {
            try
            {
                using var segmentBrush = new SolidBrush(color);
                graphics.FillPie(segmentBrush, area, startAngle, sweepAngle);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to draw the segment.", ex);
            }
        }
    }
}
using RareServer.Models;
using System.Drawing;

namespace RareServer.Service.ChartServices
{
    public interface INameDrawer
    {
        void DrawNames(Graphics graphics, List<EmployeeTimeSummary> summaries, decimal totalWorked);
    }

    public class NameDrawer : INameDrawer
    {
        public void DrawNames(Graphics graphics, List<EmployeeTimeSummary> summaries, decimal totalWorked)
        {
            try
            {
                float nameYPosition = 520; // Adjust as needed
                using var nameFont = new Font("Arial", 12);
                using var brush = new SolidBrush(Color.Black);

                foreach (var summary in summaries)
                {
                    string nameText = $"{summary.EmployeeName}: {Math.Round((summary.TotalTimeWorked / totalWorked) * 100, 1)}%";
                    graphics.DrawString(nameText, nameFont, brush, 10, nameYPosition);
                    nameYPosition += 25;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to draw names.", ex);
            }
        }
    }
}
using RareServer.Models;
using RareServer.Service.ChartServices;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;

public interface IChartService
{
    byte[] GeneratePieChart(List<EmployeeTimeSummary> summaries);
}

public class ChartService : IChartService
{
    private readonly int width = 500;
    private readonly int height = 900;
    private readonly int chartHeight = 500;
    private float startAngle = 0f;
    private readonly Color[] colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple };
    private int colorIndex = 0;
    private readonly IChartSegmentDrawer _chartSegmentDrawer;
    private readonly IPercentageTextDrawer _percentageTextDrawer;
    private readonly INameDrawer _nameDrawer;

    public ChartService(IChartSegmentDrawer chartSegmentDrawer, IPercentageTextDrawer percentageTextDrawer, INameDrawer nameDrawer)
    {
        _chartSegmentDrawer = chartSegmentDrawer;
        _percentageTextDrawer = percentageTextDrawer;
        _nameDrawer = nameDrawer;
    }

    public byte[] GeneratePieChart(List<EmployeeTimeSummary> summaries)
    {
        try
        {
            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            decimal totalWorked = summaries.Sum(s => s.TotalTimeWorked);

            DrawChart(graphics, summaries, totalWorked);
            _nameDrawer.DrawNames(graphics, summaries, totalWorked);

            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate pie chart.", ex);
        }
    }

    private void DrawChart(Graphics graphics, List<EmployeeTimeSummary> listOfTimeSummary, decimal totalWorked)
    {
        try
        {
            foreach (var summary in listOfTimeSummary)
            {
                float sweepAngle = (float)(360 * (summary.TotalTimeWorked / totalWorked));
                _chartSegmentDrawer.DrawSegment(graphics, new Rectangle(0, 0, width, chartHeight), startAngle, sweepAngle, colors[colorIndex % colors.Length]);

                float midAngle = startAngle + sweepAngle / 2;
                double radians = midAngle * (Math.PI / 180);

                float textX = (float)(width / 2 + (chartHeight / 2.5) * Math.Cos(radians));
                float textY = (float)(chartHeight / 2 + (chartHeight / 2.5) * Math.Sin(radians));

                string percentageText = $"{Math.Round((summary.TotalTimeWorked / totalWorked) * 100, 1)}%";
                _percentageTextDrawer.DrawPercentageText(graphics, percentageText, textX, textY);

                startAngle += sweepAngle;
                colorIndex++;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to draw the chart.", ex);
        }
    }
}
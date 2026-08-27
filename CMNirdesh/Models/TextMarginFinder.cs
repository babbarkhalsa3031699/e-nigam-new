using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class TextMarginFinder : IRenderListener
    {
        private float llx = float.MaxValue; // Lower-left x (left margin)
        private float lly = float.MaxValue; // Lower-left y (bottom margin)
        private float urx = float.MinValue; // Upper-right x (right margin)
        private float ury = float.MinValue; // Upper-right y (  private List<float> lineHeights = new List<float>();
        // Start of text block processing
        private List<float> lineHeights = new List<float>();
        private float lastYPosition = float.NaN;

        public void BeginTextBlock() { }

        public void EndTextBlock() { }

        public void RenderImage(ImageRenderInfo renderInfo) { }

        public void RenderText(TextRenderInfo renderInfo)
        {
            // Extract the Y-coordinate of the baseline of the current text
            Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
            float curY = curBaseline[1];

            // If this is the first text, or we're on a new line (new Y position), record the Y position
            if (float.IsNaN(lastYPosition) || Math.Abs(lastYPosition - curY) > 1f)
            {
                lineHeights.Add(curY);
                lastYPosition = curY;
            }
        }

        // Get the left boundary of the text content
        public float GetLeft()
        {
            return llx;
        }

        // Get the right boundary of the text content
        public float GetRight()
        {
            return urx;
        }

        // Get the top boundary of the text content
        public float GetTop()
        {
            return ury;
        }

        // Get the bottom boundary of the text content
        public float GetBottom()
        {
            return lly;
        }

        public List<float> GetLineHeights()
        {
            return lineHeights.OrderByDescending(y => y).ToList();  // Ensure top-to-bottom order
        }
    }

}
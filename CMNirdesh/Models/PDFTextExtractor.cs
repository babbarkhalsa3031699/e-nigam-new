using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace CMNirdesh.Models
{


    public class PDFTextExtractor : ITextExtractionStrategy
    {
        public List<float> YCoordinates { get; private set; } = new List<float>();

        public void RenderText(TextRenderInfo renderInfo)
        {
            // Get the y-coordinate of the baseline of the text
            float yStart = renderInfo.GetBaseline().GetStartPoint()[1];
            YCoordinates.Add(yStart); // Add the y-coordinate to the list
        }

        public string GetResultantText() => string.Empty; // Not used here
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo) { }



        public static float GetPreviousLineY(byte[] pdfDocument, int pageNumber, float footerHeight)
        {
            PdfReader reader = new PdfReader(pdfDocument);
            PDFTextExtractor extractor = new PDFTextExtractor();
            PdfReaderContentParser parser = new PdfReaderContentParser(reader);

            // Parse the content of the specified page
            parser.ProcessContent(pageNumber, extractor);

            // Get the bottom-most content position (the last rendered line)
            float bottomY = extractor.YCoordinates.Count > 0 ? extractor.YCoordinates.Max() : 0;

            // Find the y-coordinate of the line just above the bottom-most line
            float previousLineY = 0;
            foreach (var lineY in extractor.YCoordinates)
            {
                // Only consider lines that are above the footer and below the bottom line
                if (lineY < bottomY && lineY > footerHeight)
                {
                    previousLineY = lineY; // Update previousLineY
                    break; // Exit after finding the first valid line
                }
            }

            // Close the reader
            reader.Close();

            return previousLineY; // Return the y-coordinate of the previous line
        }

    }
    

}
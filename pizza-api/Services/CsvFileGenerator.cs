using pizza_api.Models;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 

namespace pizza_api.Services
{
    public class CsvFileGenerator
    {

        public void GenerateCsvFile(string fileName, IEnumerable<CampaignRecipient> recipients)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name must not be null or empty.", nameof(fileName));
            }

            string filePath = Path.Combine("C:\\Downloads\\", fileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Open the file for streaming
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8, bufferSize: 65536))
            {
                // Write the header row
                writer.WriteLine("Email,Phone,CampaignIdentifier,Message,Sent,SentConfirmed,ProcessedBy,CampaignId");

                // Write data in batches to avoid memory overflow
                const int batchSize = 10000; // Process rows in batches of 10,000
                foreach (var batch in recipients.Batch(batchSize))
                {
                    foreach (var recipient in batch)
                    {
                        var row = string.Join(",",
                            recipient.Email,
                            recipient.Phone,
                            recipient.CampaignIdentifier,
                            EscapeForCsv(recipient.Message),
                            recipient.Sent,
                            recipient.SentConfirmed,
                            recipient.ProcessedBy,
                            recipient.Campaign?.Id);

                        writer.WriteLine(row);
                    }

                    // Flush to ensure data is written incrementally
                    writer.Flush();
                }
            }

            Console.WriteLine($"CSV file created at: {filePath}");
        }

        // Helper method to escape data for CSV
        private string EscapeForCsv(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return '"' + value.Replace("\"", "\"\"") + '"';
            }
            return value;
        }

        

    }

    // Extension method for batching (Linq style)
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than 0.");

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, size);
                }
            }
        }

        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int size)
        {
            do
            {
                yield return source.Current;
            } while (--size > 0 && source.MoveNext());
        }
    }
}

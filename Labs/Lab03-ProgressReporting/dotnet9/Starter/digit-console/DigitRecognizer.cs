using digits;

namespace digit_console;

public static class DigitRecognizer
{
    public static async Task Run(Classifier classifier,
        DigitImage[] validation, List<Prediction> errorLog)
    {
        int totalRecords = validation.Count();
        for (int i = 0; i < totalRecords; i++)
        {
            var imageData = validation[i];
            var result = await classifier.Predict(new(imageData.Value, imageData.Image));

            if (result.Actual.Value != result.Predicted.Value)
            {
                errorLog.Add(result);
            }
        }
    }
}

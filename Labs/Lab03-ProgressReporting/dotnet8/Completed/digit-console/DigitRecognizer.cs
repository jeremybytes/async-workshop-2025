using digits;

namespace digit_console;

public record RecognizerProgress(int currentCount, Prediction prediction) { }

public static class DigitRecognizer
{
    public static Task Run(Classifier classifier,
        DigitImage[] validation, List<Prediction> errorLog)
    {
        return Run(classifier, validation, errorLog, null);
    }

    //public static async Task Run(Classifier classifier,
    //    DigitImage[] validation, List<Prediction> errorLog,
    //    IProgress<int>? progress)
    //{
    //    int totalRecords = validation.Count();
    //    for (int i = 0; i < totalRecords; i++)
    //    {
    //        var imageData = validation[i];
    //        var result = await classifier.Predict(new(imageData.Value, imageData.Image));

    //        progress?.Report(i + 1);

    //        if (result.Actual.Value != result.Predicted.Value)
    //        {
    //            errorLog.Add(result);
    //        }
    //    }
    //}

    public static async Task Run(Classifier classifier,
        DigitImage[] validation, List<Prediction> errorLog,
        IProgress<RecognizerProgress>? progress)
    {
        int totalRecords = validation.Count();
        for (int i = 0; i < totalRecords; i++)
        {
            var imageData = validation[i];
            var result = await classifier.Predict(new(imageData.Value, imageData.Image));

            progress?.Report(new (i + 1, result));

            if (result.Actual.Value != result.Predicted.Value)
            {
                errorLog.Add(result);
            }
        }
    }
}

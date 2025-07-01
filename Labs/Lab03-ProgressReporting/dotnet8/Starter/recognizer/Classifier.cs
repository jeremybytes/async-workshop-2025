namespace digits;

public record DigitImage(int Value, int[] Image);
public record Prediction(DigitImage Actual, DigitImage Predicted);

public abstract class Classifier
{
    public string Name { get; set; }
    public DigitImage[] TrainingData { get; set; }

    public Classifier(string name, DigitImage[] trainingData)
    {
        Name = name;
        TrainingData = trainingData;
    }

    public abstract Task<Prediction> Predict(DigitImage input);
}


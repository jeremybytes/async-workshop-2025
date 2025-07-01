namespace digits;

public class ManhattanClassifier : Classifier
{
    public ManhattanClassifier(DigitImage[] training_data) :
        base("Manhattan Classifier", training_data)
    {
    }

    public override Task<Prediction> Predict(DigitImage input)
    {
        return Task.Run(() =>
        {
            int[] inputImage = input.Image;
            int best_total = int.MaxValue;
            DigitImage best = new(0, Array.Empty<int>());
            foreach (DigitImage candidate in TrainingData)
            {
                int total = 0;
                int[] candidateImage = candidate.Image;
                for (int i = 0; i < 784; i++)
                {
                    int diff = inputImage[i] - candidateImage[i];
                    if (diff < 0)
                        diff *= -1;
                    total += diff;
                }
                if (total < best_total)
                {
                    best_total = total;
                    best = candidate;
                }
            }

            return new Prediction(input, best);
        });
    }
}

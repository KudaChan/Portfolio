from keras.models import load_model  # type: ignore
import numpy as np
from PIL import Image
import pandas as pd
import os
import warnings
import matplotlib.pyplot as plt

warnings.filterwarnings("ignore")


def predict_plant(image_path, model):
    result = []
    img = Image.open(image_path)
    if 'InceptionV3' in model.name or 'Inception_ResNet' in model.name:
        img = img.resize((299, 299))
    elif 'DenseNet' in model.name or 'VGG16' in model.name:
        img = img.resize((224, 224))
    else:
        pass
    img = np.array(img)
    img = img / 255.0
    img = img.reshape(1, *img.shape)
    prediction = model.predict(img)

    class_labels = [1, 2, 3, 4, 5]

    # Get the index of the highest probability
    predicted_index = np.argmax(prediction)

    # Get the highest probability
    highest_probability = prediction[0][predicted_index]

    # Return the class label with the highest probability and the probability
    result.append((class_labels[predicted_index], highest_probability))
    return result


def predict():
    img_folder_path = r"D:\Dataset\medai\test"
    species_dict = {1: 'Aloevera', 2: 'Amla',
                    3: 'Amruthaballi', 4: 'Arali', 5: 'Ashoka'}
    models_dir = r'D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\models_files'
    model_files = os.listdir(models_dir)
    img_files = os.listdir(img_folder_path)

    # Load all models at the start
    models = {model_file: load_model(os.path.join(
        models_dir, model_file)) for model_file in model_files}

    # Initialize DataFrame with image names as index and model names as columns
    results_df = pd.DataFrame(index=img_files, columns=model_files)

    for i, model_file in enumerate(model_files):
        model = models[model_file]
        for img in img_files:
            img_path = os.path.join(img_folder_path, img)
            predictions = predict_plant(img_path, model)
            prediction, probability = predictions[0]
            species = species_dict[int(prediction)]
            # Store the prediction in the DataFrame
            results_df.loc[img, model_file] = species

    mode_result = results_df.mode(axis=1)

    results_df['Majority'] = mode_result.apply(
        lambda row: np.random.choice(row[~pd.isnull(row)]), axis=1)
    # Print the final DataFrame
    print(results_df)

    for img in img_files:
        plt.figure(figsize=(10, 5))
        results_df.loc[img].value_counts().plot(kind='bar')
        plt.title(f'Predictions for {img}')
        plt.xlabel('Species')
        plt.ylabel('Count')
        plt.show()


if __name__ == "__main__":
    predict()

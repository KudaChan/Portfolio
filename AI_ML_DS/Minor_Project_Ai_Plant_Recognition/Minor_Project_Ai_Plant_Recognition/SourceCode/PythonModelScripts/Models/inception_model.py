"""_summary_
This is the implementation of inception model.
"""
from tensorflow.keras.applications.inception_v3 import InceptionV3  # type: ignore
from tensorflow.keras.models import Model  # type: ignore
from tensorflow.keras.layers import Dense, GlobalAveragePooling2D  # type: ignore
from tensorflow.keras.utils import to_categorical  # type: ignore
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau, ModelCheckpoint  # type: ignore
from keras.layers import Dense, Dropout, BatchNormalization  # type: ignore
from keras.optimizers import Adam  # type: ignore
import matplotlib.pyplot as plt
from PIL import Image
import numpy as np
import sys

try:
    sys.path.append('SourceCode/PythonModelScripts')
    import model_data_prepare as mdp
except ImportError:
    print("Unable to import model_data_prepare")


class InceptionModel:
    def __init__(self, train_data, test_data, i):
        self.test_data = test_data
        self.train_data = train_data
        self.processed_data_type = i

    def data_prepare(self):
        x_train = []
        y_train = []

        for img in self.train_data:
            image = Image.open(img.img_path)
            image = image.resize((299, 299))
            image = np.array(image, dtype='float16') / 255.0
            x_train.append(image)

            y_train.append(to_categorical(img.species_id-1, num_classes=89))

        x_test = []
        y_test = []
        for img in self.test_data:
            # Open and resize the image
            image = Image.open(img.img_path)
            # resize to the input size that your model expects
            image = image.resize((299, 299))
            image = np.array(image) / 255.0  # normalize pixel values to [0, 1]
            x_test.append(image)

            # Convert label to one-hot encoding
            y_test.append(to_categorical(img.species_id-1, num_classes=89))

        return np.array(x_train), np.array(y_train), np.array(x_test), np.array(y_test)

    def model(self):
        base_model = InceptionV3(weights='imagenet', include_top=False)

        x = base_model.output
        x = GlobalAveragePooling2D()(x)

        x = Dense(2048, activation='relu')(x)
        x = BatchNormalization()(x)  # Add batch normalization layer
        x = Dropout(0.5)(x)  # Add dropout layer to prevent overfitting

        x = Dense(1024, activation='relu')(x)
        x = BatchNormalization()(x)  # Add batch normalization layer
        x = Dropout(0.5)(x)

        x = Dense(512, activation='relu')(x)
        x = BatchNormalization()(x)  # Add batch normalization layer
        x = Dropout(0.5)(x)

        predictions = Dense(89, activation='softmax')(x)

        model = Model(inputs=base_model.input, outputs=predictions)

        for layer in base_model.layers:
            layer.trainable = True

        optimizer = Adam(learning_rate=0.0001)

        model.compile(optimizer=optimizer,
                      loss='categorical_crossentropy', metrics=['accuracy'])

        return model

    def train_model(self, x_train, y_train, x_test, y_test):
        model = self.model()

        # Early stopping
        early_stop = EarlyStopping(monitor='val_loss', patience=10)

        # Reduce learning rate when a metric has stopped improving
        lr_reduce = ReduceLROnPlateau(
            monitor='val_loss', factor=0.1, patience=2, verbose=1, min_delta=1e-4)

        # Save the best model after every epoch
        checkpoint = ModelCheckpoint(
            f'{self.processed_data_type}.keras', verbose=1, save_best_only=True)

        # Train the model
        history = model.fit(x_train, y_train, validation_data=(
            x_test, y_test), epochs=100, batch_size=32, callbacks=[early_stop, lr_reduce, checkpoint])

        # Plot training & validation accuracy values
        plt.figure(figsize=(12, 4))
        plt.subplot(1, 2, 1)
        plt.plot(history.history['accuracy'])
        plt.plot(history.history['val_accuracy'])
        plt.title('Model accuracy')
        plt.ylabel('Accuracy')
        plt.xlabel('Epoch')
        plt.legend(['Train', 'Test'], loc='upper left')

        # Plot training & validation loss values
        plt.subplot(1, 2, 2)
        plt.plot(history.history['loss'])
        plt.plot(history.history['val_loss'])
        plt.title('Model loss')
        plt.ylabel('Loss')
        plt.xlabel('Epoch')
        plt.legend(['Train', 'Test'], loc='upper left')

        plt.tight_layout()
        plt.suptitle(self.processed_data_type, fontsize=16)

        save_path = r"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\Results"
        name = f"model_accuracy_loss_{self.processed_data_type}.png"
        save_path = save_path + "/" + name
        plt.savefig(save_path)
        plt.close()


# if __name__ == "__main__":
#     data_parser = mdp.Main()
#     temp = [0]
#     for i in temp:
#         print(i)
#         traindata, testdata, pname = data_parser.data_prepare_starter(i)

#         print(f"staring training for {pname}")
#         inception_model = InceptionModel(traindata, testdata, pname)
#         x_train, y_train, x_test, y_test = inception_model.data_prepare()
#         inception_model.train_model(x_train, y_train, x_test, y_test)

"""_summary_
This is the implementation of inception resnet v3 model.
"""
from sklearn.metrics import r2_score, mean_squared_error
from tensorflow.keras.applications.inception_resnet_v2 import InceptionResNetV2  # type: ignore
from tensorflow.keras.models import Model  # type: ignore
from tensorflow.keras.layers import Dense, GlobalAveragePooling2D  # type: ignore
from tensorflow.keras.utils import to_categorical  # type: ignore
from tensorflow.keras.callbacks import EarlyStopping, ReduceLROnPlateau, ModelCheckpoint  # type: ignore
from keras.layers import Dense, Dropout, BatchNormalization  # type: ignore
from keras.optimizers import Adam  # type: ignore
import matplotlib.pyplot as plt
from PIL import Image
import numpy as np


class InceptionResNetModel:
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

            y_train.append(to_categorical(img.species_id-1, num_classes=5))

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
            y_test.append(to_categorical(img.species_id-1, num_classes=5))

        return np.array(x_train), np.array(y_train), np.array(x_test), np.array(y_test)

    def model(self):
        base_model = InceptionResNetV2(weights='imagenet', include_top=False)

        x = base_model.output
        x = GlobalAveragePooling2D()(x)

        x = Dense(2048, activation='relu')(x)
        x = BatchNormalization()(x)  # Add batch normalization layer
        x = Dropout(0.6)(x)  # Add dropout layer to prevent overfitting

        predictions = Dense(5, activation='softmax')(x)

        model = Model(inputs=base_model.input, outputs=predictions)

        for layer in base_model.layers:
            layer.trainable = False

        optimizer = Adam(learning_rate=0.01)

        model.compile(optimizer=optimizer,
                      loss='categorical_crossentropy', metrics=['accuracy'])

        return model

    def train_model(self, x_train, y_train, x_test, y_test):
        model = self.model()

        # Early stopping
        early_stop = EarlyStopping(monitor='val_loss', patience=5)

        # Reduce learning rate when a metric has stopped improving
        lr_reduce = ReduceLROnPlateau(
            monitor='val_loss', factor=0.1, patience=2, verbose=1, min_delta=1e-4, min_lr=0.0001)

        # Save the best model after every epoch
        checkpoint = ModelCheckpoint(
            f'Inception_ResNet_{self.processed_data_type}.keras', monitor='val_loss', verbose=1, save_best_only=True)

        # Train the model
        history = model.fit(x_train, y_train, validation_data=(
            x_test, y_test), epochs=10, batch_size=32, callbacks=[early_stop, lr_reduce, checkpoint])

        # After training the model
        y_pred_train = model.predict(x_train)
        y_pred_test = model.predict(x_test)

        # Calculate R-squared and mean R-squared value for the training set
        r2_train = r2_score(y_train, y_pred_train)
        mean_r2_train = mean_squared_error(y_train, y_pred_train)

        # Calculate R-squared and mean R-squared value for the test set
        r2_test = r2_score(y_test, y_pred_test)
        mean_r2_test = mean_squared_error(y_test, y_pred_test)

        print(f'Training R2: {r2_train}, Mean R2: {mean_r2_train}')
        print(f'Test R2: {r2_test}, Mean R2: {mean_r2_test}')

        # Evaluate the model's performance on the training set
        train_score = model.evaluate(x_train, y_train, verbose=0)
        print(f'Train loss: {train_score[0]
                             }, Train accuracy: {train_score[1]}')

        # Evaluate the model's performance on the test set
        test_score = model.evaluate(x_test, y_test, verbose=0)
        print(f'Test loss: {test_score[0]}, Test accuracy: {test_score[1]}')

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
        name = f"Inception_ResNet_model_accuracy_loss_{
            self.processed_data_type}.png"
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

"""_summary_
This is the implementation of inception model.
"""

import torch
import timm
from torch.utils.data import DataLoader
from torchvision import transforms
from torch.utils.data import Dataset
import matplotlib.pyplot as plt
from sklearn.metrics import r2_score, mean_squared_error
import numpy as np
from tqdm import tqdm
from PIL import Image
import sys

try:
    sys.path.append("SourceCode/PythonModelScripts")
    import model_data_prepare as mdp
except ImportError:
    print("Unable to import model_data_prepare")
import os

os.environ["TF_ENABLE_ONEDNN_OPTS"] = "0"

class CustomDataset(Dataset):
    def __init__(self, data, transform=None):
        self.data = data
        self.transform = transform

    def __len__(self):
        return len(self.data)

    def __getitem__(self, idx):
        img_data = self.data[idx]
        # print(f"Data at index {idx}: {self.data[idx]}")
        image = Image.open(img_data.img_path)
        label = img_data.species_id - 1
        if self.transform:
            image = self.transform(image)
        return image, label

class DataPreparation:
    def __init__(self, data_parser, process_id):
        self.data_parser = data_parser
        self.process_id = process_id

    def prepare_data(self):
        traindata, testdata, pname = self.data_parser.data_prepare_starter(
            self.process_id
        )
        transform = transforms.Compose(
            [
                transforms.RandomHorizontalFlip(),
                transforms.RandomRotation(10),
                transforms.RandomResizedCrop(224),
                transforms.Resize(224, 224),
                transforms.ToTensor(),
                transforms.Normalize(
                    mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]
                ),
            ]
        )
        classes = 10
        train_dataset = CustomDataset(traindata, transform)
        test_dataset = CustomDataset(testdata, transform)

        train_loader = DataLoader(train_dataset, batch_size=32, shuffle=True)
        test_loader = DataLoader(test_dataset, batch_size=32, shuffle=False)

        return train_loader, test_loader, classes

class ViTModel:
    def __init__(self, num_classes):
        self.model = timm.create_model(
            "vit_base_patch16_224", pretrained=True, num_classes=num_classes
        )
        self.device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
        self.model = self.model.to(self.device)
        self.criterion = torch.nn.CrossEntropyLoss()
        self.optimizer = torch.optim.Adam(self.model.parameters())

    def train(self, train_loader, num_epochs):
        self.losses = []
        for epoch in range(num_epochs):
            print(epoch)
            for inputs, labels in tqdm(train_loader, desc="Training", leave=False):
                inputs = inputs.to(self.device)
                labels = labels.to(self.device)

                self.optimizer.zero_grad()

                outputs = self.model(inputs)
                loss = self.criterion(outputs, labels)
                loss.backward()
                self.optimizer.step()

                self.losses.append(loss.item())

    def evaluate(self, test_loader):
        self.model.eval()
        self.accuracies = []

        with torch.no_grad():
            correct = 0
            total = 0
            for inputs, labels in tqdm(test_loader, desc="Testing", leave=False):
                inputs = inputs.to(self.device)
                labels = labels.to(self.device)

                outputs = self.model(inputs)

                _, predicted = torch.max(outputs.data, 1)
                total += labels.size(0)
                correct += (predicted == labels).sum().item()
                accuracy = 100 * correct / total
                self.accuracies.append(accuracy)

        print(f"Correct Prediction : {correct}")
        print(f"Total Prediction : {total}")
        print("Test Accuracy: {} %".format(100 * correct / total))

class VITMain:
    def vit_main(self):
        data_parser = mdp.Main()
        process_id = 0
        num_epochs = 10
        data_prep = DataPreparation(data_parser, process_id)
        train_loader, test_loader, num_classes = data_prep.prepare_data()

        vit_model = ViTModel(num_classes)
        vit_model.train(train_loader, num_epochs)
        vit_model.evaluate(test_loader)

        torch.save(vit_model.model.state_dict(), "VIT_model.pth")

        plt.figure(figsize=(12, 4))
        plt.subplot(1, 2, 1)
        plt.plot(vit_model.losses)
        plt.title("Training Loss")
        plt.subplot(1, 2, 2)
        plt.plot(vit_model.accuracies)
        plt.title("Test Accuracy")

        save_path = r"D:\Project\AI_ML_DS\Minor_Project_Ai_Plant_Recognition\Minor_Project_Ai_Plant_Recognition\Results"
        name = "VIT_model_accuracy_loss.png"
        save_path = save_path + "/" + name
        plt.savefig(save_path)
        plt.close()
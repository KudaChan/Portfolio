"""_summary_
imports
"""
from Models.inception_model import InceptionModel
import model_data_prepare as mdp


if __name__ == "__main__":
    print("Hello, world!")

    data_parser = mdp.Main()

    temp = [0]
    for i in temp:
        print(i)
        traindata, testdata, pname = data_parser.data_prepare_starter(i)

        print(f"staring training for {pname}")
        inception_model = InceptionModel(traindata, testdata, pname)
        x_train, y_train, x_test, y_test = inception_model.data_prepare()
        inception_model.train_model(x_train, y_train, x_test, y_test)

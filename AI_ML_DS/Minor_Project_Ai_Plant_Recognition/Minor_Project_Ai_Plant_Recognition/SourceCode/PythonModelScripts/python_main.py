"""_summary_
imports
"""

from Models.inception_model import InceptionModel
from Models.inception_resnetV2_model import InceptionResNetModel
from Models.resnet_model import ResnetModel
from Models.vgg16_model import VGGModel
from Models.vit_model import VITMain
from Models.swin_model import SWINMain
import model_data_prepare as mdp

if __name__ == "__main__":
    print("Hello, world!")

    data_parser = mdp.Main()
    traindata, testdata, pname = data_parser.data_prepare_starter(0)

    # print("staring training InceptionV3")
    # inception_model = InceptionModel(traindata, testdata, pname)
    # x_train, y_train, x_test, y_test = inception_model.data_prepare()
    # inception_model.train_model(x_train, y_train, x_test, y_test)

    # print("staring training InceptionResNetV2")
    # inception_resnetV2_model = InceptionResNetModel(traindata, testdata, pname)
    # x_train, y_train, x_test, y_test = inception_resnetV2_model.data_prepare()
    # inception_resnetV2_model.train_model(x_train, y_train, x_test, y_test)

    # print("staring training ResNet50")
    # resnet50_model = ResnetModel(traindata, testdata, pname)
    # x_train, y_train, x_test, y_test = resnet50_model.data_prepare()
    # resnet50_model.train_model(x_train, y_train, x_test, y_test)

    # print("staring training VGG16")
    # vgg16_model = VGGModel(traindata, testdata, pname)
    # x_train, y_train, x_test, y_test = vgg16_model.data_prepare()
    # vgg16_model.train_model(x_train, y_train, x_test, y_test)

    print("starting training VIT Model")
    vit_mod = VITMain()
    vit_mod.vit_main()

    print("starting training SWIN Model")
    swin_mod = SWINMain()
    swin_mod.swin_main()
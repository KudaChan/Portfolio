"""_summary_
imports
"""
import model_data_prepare as mdp


if __name__ == "__main__":
    print("Hello, world!")

    data_prepare = mdp.Main()

    for i in range(0, 6):
        data_prepare.data_prepare_starter(i)

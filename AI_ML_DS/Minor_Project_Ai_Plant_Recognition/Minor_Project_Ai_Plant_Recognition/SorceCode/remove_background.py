from rembg import remove
from PIL import Image
from io import BytesIO
import numpy as np


def remove_background(image_path):
    with open(image_path, "rb") as input_img_file:
        input_img = input_img_file.read()
        back_remove = remove(input_img, alpha_matting_background_threshold=200)

        if isinstance(back_remove, Image.Image):
            img_png = back_remove.convert("RGBA")
        elif isinstance(back_remove, np.ndarray):
            img_png = Image.fromarray(back_remove).convert("RGBA")
        else:
            img_png = Image.open(BytesIO(back_remove)).convert("RGBA")

        print("Background Removed: ok")


# return np.array(img_png)

# x = 1
# y = 2
# z = x + y

# print(z)
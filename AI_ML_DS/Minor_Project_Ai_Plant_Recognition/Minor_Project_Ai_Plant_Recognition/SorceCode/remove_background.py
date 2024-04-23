from rembg import remove
from PIL import Image
from io import BytesIO
import numpy as np
import os

def remove_backgrnd(image_path, image_output_path):
    """
    Function to remove the background of an image.

    Parameters:
    image_path (str): The path of the image file from which the background is to be removed.
    image_output_path (str): The path where the image with the background removed is to be saved.

    Returns:
    None
    """
    with open(image_path, "rb") as input_img_file:
        input_img = input_img_file.read()
        back_remove = remove(input_img, alpha_matting_background_threshold=200)

        img = Image.open(BytesIO(back_remove)).convert("RGB")

        os.makedirs(os.path.dirname(image_output_path), exist_ok=True)

        img.save(image_output_path, "JPEG")

    print("Background Removed: ok")
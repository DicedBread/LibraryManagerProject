import pandas as pd


df = pd.read_csv("books.csv", sep=";", index_col=0, on_bad_lines="warn", low_memory=False)
df['Year-Of-Publication'] = pd.to_datetime(df['Year-Of-Publication'], errors='coerce')
df = df.drop(df.index[20:])
df.drop("Image-URL-S", inplace=True, axis=1)
df.drop("Image-URL-L", inplace=True, axis=1)
df.to_csv("out.csv")
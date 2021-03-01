import pandas as pd
print('Succesfull inport pandas')
csvdat = pd.read_csv('BIM PARAMS 2021.csv')
print(csvdat[['NAME','DATATYPE']])
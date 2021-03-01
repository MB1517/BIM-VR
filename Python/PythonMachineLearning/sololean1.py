import matplotlib.pyplot as plt
import pandas as pd

from sklearn.linear_model import LogisticRegression


# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# print(df.head())


# pd.options.display.max_columns = 6
# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# print(df.describe())


# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# col = df['Fare']
# print(col)

# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# small_df = df[['Age',  'Sex', 'Survived']]
# print(small_df.head())

# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# print(df['Sex'] == 'male')

# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# df['male'] = df['Sex'] == 'male'
# print(df.head())

# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# print(df['Fare'].values)

# import pandas as pd
# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# print(df[['Pclass', 'Fare', 'Age']].values)

# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# arr = df[['Pclass', 'Fare', 'Age']].values
# print(arr.shape)

# df = pd.read_csv('https://sololearn.com/uploads/files/titanic.csv')
# # take first 10 values for simplicity
# arr = df[['Pclass', 'Fare', 'Age']].values[:10]

# mask = arr[:, 2] < 18
# # print(arr[mask])
# print(arr[arr[:, 2] < 18])

# arr = df[['Pclass', 'Fare', 'Age']].values
# mask = arr[:, 2] < 18

# print(mask.sum())
# print((arr[:, 2] < 18).sum())


# plt.scatter(df['Age'], df['Fare'])
# plt.scatter(df['Age'], df['Fare'], c=df['Pclass'])
# plt.xlabel('Age')
# plt.ylabel('Fare')

# plt.plot([0, 80], [85, 5])

# plt.show()

# df['male'] = df['Sex'] == 'male'
# X = df[['Pclass', 'male', 'Age', 'Siblings/Spouses', 'Parents/Children', 'Fare']].values
# y = df['Survived'].values
# print(X)
# print(y)

# model = LogisticRegression()
# X = df[['Fare', 'Age']].values
# y = df['Survived'].values
# model.fit(X, y)
# print(model.coef_, model.intercept_)

# plt.scatter( df['Fare'], df['Age'], c=df['Survived'])
# plt.xlabel('Fare')
# plt.ylabel('Age')

# plt.plot([0, 80], [85, 5])

# plt.show()


# df['male'] = df['Sex'] == 'male'
# X = df[['Pclass', 'male', 'Age', 'Siblings/Spouses', 'Parents/Children', 'Fare']].values
# y = df['Survived'].values
# model = LogisticRegression()
# model.fit(X, y)
# print(model.predict(X))
# print(model.predict([[3, True, 22.0, 1, 0, 7.25]]))
# print(model.predict(X[:5]))
# print(y[:5])


# df['male'] = df['Sex'] == 'male'
# X = df[['Pclass', 'male', 'Age', 'Siblings/Spouses', 'Parents/Children', 'Fare']].values
# y = df['Survived'].values

# model = LogisticRegression()
# model.fit(X, y)

# y_pred = model.predict(X)
# print((y == y_pred).sum())
# print(model.score(X, y))

from sklearn.datasets import load_breast_cancer
cancer_data = load_breast_cancer()
# print(cancer_data.keys())
# print(cancer_data['DESCR'])
# print(cancer_data['data'].shape)
df = pd.DataFrame(cancer_data['data'], columns=cancer_data['feature_names'])
df['target'] = cancer_data['target']
# print(df.head())
X = df[cancer_data['feature_names']].values
y = df['target'].values
model = LogisticRegression(solver='liblinear')
model.fit(X, y)
print("prediction for datapoint 0:", model.predict([X[0]]))
print(model.score(X, y))
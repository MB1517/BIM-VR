import matplotlib.pyplot as plt
import numpy as np

x = 10*np.random.rand(200,1)
y = (0.2 + 0.8*x) * np.sin(2*np.pi*x) + np.random.randn(200,1)

plt.hist(y,bins=20)
plt.show()
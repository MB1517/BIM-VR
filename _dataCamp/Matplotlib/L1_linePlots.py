import matplotlib.pyplot as plt
import numpy as np

x = np.linspace(0,1,201)
y = np.sin((2*np.pi*x)**2)

plt.plot(x,y,'red')
plt.show()
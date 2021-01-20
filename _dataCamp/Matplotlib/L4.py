import matplotlib.pyplot as plt
import numpy as np

temperature = 10*np.random.rand(200,1)

dewpoint = 10*np.random.rand(200,1)

plt.plot(t,temperature,'red')
plt.plot(t,dewpoint,'blue')

plt.xlable('Date')
plt.title('Temperature * Dew Point')

plt.show()
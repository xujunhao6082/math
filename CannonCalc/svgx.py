import math
import GunModel
import sys
color = "black"
w, h = 1000, 1000
central = 0, 10
diffk = 10
data = []
###################################
k, m, v_0, g = 0.2, 5, 800, 32.81
gm = GunModel.GunModel(k, m, v_0, g, (0.0001,0.0001))
θ = gm.calc(100,-4.5)
if θ[0] is None:
    print("无解")
    sys.exit()
if θ[1] is None:
    θ = (θ[0],θ[0])
print(f"θ:{θ}({(math.degrees(θ[0]),math.degrees(θ[1]))}°)")
insides = []
for t in θ:
    dt = 0.001
    for i in range(10000):
        x = m / k * v_0 * math.cos(t)*(1 - math.exp((-k) / m*i*dt))
        y = m / k * (v_0*math.sin(t) + m*g / k) * \
            (1 - math.exp(-k / m*i*dt)) - m / k * g * i*dt
        data.append(f"{x*diffk+central[0]} {h-y*diffk+central[1]}")
    ###################################
    inside = " L".join(data)
    inside = f"<path d=\"M0 {h-central[1]} H {h}\" stroke=\"red\" stroke-width=\"10\"/>,<path d=\"M{central[0]} 0 V {w}\" stroke=\"red\" stroke-width=\"10\"/><path d=\"M{inside}\" stroke=\"{color}\" stroke-width=\"3\"  fill=\"none\" />"
    insides.append(inside)
s = f"<svg version=\"1.1\"\n  baseProfile=\"full\"\n  width=\"{w}\"\n height=\"{h}\"\n  xmlns=\"http://www.w3.org/2000/svg\">{insides[0]}{insides[1]}</svg>"
with open("a.svg", 'w') as f:
    f.write(s)

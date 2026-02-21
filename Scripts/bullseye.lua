local rings = 7

for i = rings, 1, -1 do
    local color = (i % 2 == 0) and "#FFFFFF" or "#FF0000"
    turtle.pen_size(i * 22)
    turtle.color(color)
    turtle.pen_up()
    turtle.center()
    turtle.pen_down()
    turtle.move(1)
end
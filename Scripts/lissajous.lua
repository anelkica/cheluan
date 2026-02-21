local function lissajous(a, b, scale, hex)
    turtle.pen_up()
    turtle.center()
    turtle.pen_down()
    turtle.color(hex)
    for i = 0, 628 do
        local t = i / 100.0
        local x = scale * math.sin(a * t)
        local y = scale * math.sin(b * t)
        turtle.pen_up()
        turtle.teleport(x, y)
        turtle.pen_down()
        turtle.move(1)
    end
end

lissajous(3, 2, 150, "#FFD0D0")  -- pink
lissajous(5, 4,  120, "#C8F8FF")  -- blue
lissajous(7, 6,  90, "#E3FFC2")  -- green
turtle.pen_size(1)

local function dragon(length, depth, turn_dir)
    if depth == 0 then
        turtle.move(length)
    else
        turtle.turn(-turn_dir * 45)
        dragon(length / 1.414, depth - 1, 1)
        turtle.turn(turn_dir * 90)
        dragon(length / 1.414, depth - 1, -1)
        turtle.turn(-turn_dir * 45)
    end
end

turtle.pen_up()
turtle.teleport(0, 0)
turtle.pen_down()

local colors = {"#FF71CE", "#01CDFE", "#05FFA1", "#B967FF"}

for i = 1, 4 do
    turtle.color(colors[i])
    turtle.pen_up()
    turtle.center()
    turtle.pen_down()
    turtle.angle((i - 1) * 90)
    dragon(100, 10, 1)
end
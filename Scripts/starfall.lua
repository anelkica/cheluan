local colors = {"#FF71CE", "#01CDFE", "#05FFA1", "#B967FF", "#FFFB96"}

local function draw_star(size)
    turtle.pen_down()
    for i = 1, 5 do
        turtle.move(size)
        turtle.turn(144)
    end
    turtle.pen_up()
end

for i = 1, 365 do
    local size = math.random(5, 20)

    -- keep star fully inside bounds by shrinking the spawn area by star size
    local x = math.random(-200 + size, 200 - size)
    local y = math.random(-200 + size, 200 - size)

    turtle.pen_up()
    turtle.teleport(x, y)
    turtle.angle(math.random(0, 360))
    turtle.color(colors[math.random(1, #colors)])
    draw_star(size)
end
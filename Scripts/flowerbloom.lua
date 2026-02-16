local petals = 12
local palette = {"#FF71CE", "#B967FF", "#01CDFE", "#05FFA1"}
local petalSize = 2

function draw_petal(size)
    for i = 1, 2 do
        for j = 1, 90 do
            turtle.move(size)
            turtle.turn(1)
        end
        turtle.turn(90)
    end
end

turtle.pen_size(2)

for i = 1, petals do
    local color = palette[(i % #palette) + 1]
    turtle.color(color)
    
    turtle.pen_down()
    draw_petal(petalSize)
    turtle.pen_up()
    
    turtle.turn(360 / petals)
end
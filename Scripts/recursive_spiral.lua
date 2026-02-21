-- by eebmagic (Everett Bolton
turtle.pen_size(5)

color = "#e8c660"
turtle.color(color)

local function spiral(input_len)
    if input_len <= 10 then
        turtle.turn(90)
    else
        turtle.move(input_len)
        turtle.turn(-90)
        spiral(input_len - 10)
        turtle.turn(180)
        turtle.move(input_len)
        turtle.turn(180)
        turtle.turn(90)
    end
end

for i = 1, 8 do
    turtle.turn(-45)
    spiral(100)
end
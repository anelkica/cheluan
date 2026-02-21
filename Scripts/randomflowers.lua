local STEPS      = 30
local PEN_SIZE   = 1.5
local CENTER_DOT = 8

local palettes = {
    { petal = {255, 182, 193}, center = {255, 240, 150} },  -- baby pink,   mint
    { petal = {182, 210, 255}, center = {255, 245, 180} },  -- periwinkle,  cream
    { petal = {255, 210, 230}, center = {255, 182, 193} },  -- blush,       pink
    { petal = {210, 245, 210}, center = {255, 220, 180} },  -- soft mint,   peach
    { petal = {255, 225, 180}, center = {255, 200, 210} },  -- pastel peach, lilac
    { petal = {220, 190, 255}, center = {255, 240, 200} },  -- lavender,    soft yellow
}

local function petal(length, angle_offset)
    local step_size = length / STEPS

    turtle.angle(angle_offset)
    for i = 0, STEPS do
        turtle.move(step_size)
        turtle.turn(3)
    end

    turtle.angle(angle_offset + 180)
    for i = 0, STEPS do
        turtle.move(step_size)
        turtle.turn(3)
    end
end

local function flower(x, y, petal_count, petal_length, r, g, b, center_r, center_g, center_b)
    turtle.pen_size(PEN_SIZE)
    turtle.color(r, g, b)

    for i = 0, petal_count - 1 do
        turtle.pen_up()
        turtle.teleport(x, y)
        turtle.pen_down()
        petal(petal_length, i * (360 / petal_count))
    end

    turtle.pen_up()
    turtle.teleport(x, y)
    turtle.pen_down()
    turtle.pen_size(CENTER_DOT)
    turtle.color(center_r, center_g, center_b)
    turtle.move(1)
end


for i = 1, 12 do
    local x            = math.random(-130, 130)
    local y            = math.random(-130, 130)
    local petal_count  = math.random(5, 9)
    local petal_length = math.random(20, 45)
    local palette      = palettes[math.random(#palettes)]

    flower(x, y,
        petal_count, petal_length,
        palette.petal[1],  palette.petal[2],  palette.petal[3],
        palette.center[1], palette.center[2], palette.center[3])
end
-- boss.lua

Boss = {}
Boss.__index = Boss

function Boss:new(image, x, y, width, height, velocidadeX, velocidadeY, vida)
    local boss = setmetatable({}, Boss)
    boss.image = image
    boss.x = x
    boss.y = y
    boss.width = width
    boss.height = height
    boss.velocidadeX = velocidadeX * 5
    boss.velocidadeY = velocidadeY * 5
    boss.vida = vida
    boss.direcaoX = 1
    boss.direcaoY = 1
    return boss
end

function Boss:update(dt)
    self.x = self.x + self.velocidadeX * self.direcaoX * dt
    self.y = self.y + self.velocidadeY * self.direcaoY * dt

    if self.x < 0 or self.x > love.graphics.getWidth() - self.width then
        self.direcaoX = -self.direcaoX
    end
    if self.y < 0 or self.y > love.graphics.getHeight() - self.height then
        self.direcaoY = -self.direcaoY
    end
end

function Boss:draw()
    love.graphics.draw(self.image, self.x, self.y)
end

function Boss:levaDano(damage)
    self.vida = self.vida - damage
end

function Boss:perde()
    return self.vida <= 0
end

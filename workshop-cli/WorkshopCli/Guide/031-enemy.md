
5. A seguir vamos inserir este código:

  Enemy = {xPos = love.graphics.getWidth(), yPos = 0, width = 64, height = 64}
  
  function Enemy:new (o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

A função "Enemy" pode parecer complicado, mas simplesmente regista a existência de um inimigo.


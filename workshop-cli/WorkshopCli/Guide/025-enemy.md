
Agora temos tudo sobre o jogador feito, passaremos para os inimigos.

1. Vamos começar por indicar as imagens as suas variaveis na função "load()":

ImagemInimigo = love.graphics.newImage("inimigo1.png")
inimigos = {}
spawnTimer = 0
spawnTimerMax = 0.5

2. Agora vamos desenhar na função "draw()", adiciona no fim da funçao o seguinte:

for index, inimigo in ipairs(inimigos) do
   love.graphics.draw(inimigo.img, inimigo.x, inimigo.y)
end

3. No fim do teu codigo adiciona esta função:

function updateinimigos(dt)
 spawnTimer = spawnTimer - dt
 if spawnTimer <= 0 then
  spawnTimer = spawnTimerMax
  y = love.math.random(0, love.graphics.getHeight() - 64)
  inimigo = {x = love.graphics.getWidth(), y = y, width = 64, height = 64, velocidade = 100, img = ImagemInimigo}
  table.insert(inimigos, inimigo)
 end
 for i = #inimigos, 1, -1 do
  inimigo = inimigos[i]
  inimigo.x = inimigo.x - inimigo.velocidade * dt
 end
end

4. Por fim na função "update(dt)" depois da linha "atualizarMisseis(dt)" adiciona esta linha:

   updateinimigos(dt) 

Agora Deves ter um monte de inimigos a vir em direção da tua nave. Altera o valor de "spawnTimerMax" para teres á tua escolha a quantidade de inimigos que vai aparecendo ao longo do tempo.

    

Agora temos tudo sobre o jogador feito, passaremos para os inimigos.

1. Vamos começar por indicar as imagens as suas variaveis na função "load()":

ImagemInimigo = love.graphics.newImage("inimigo1.png")
inimigos = {}
geraInimigoTempo = 0
geraInimigoTempoMax = 0.5

2. Agora vamos desenhar na função "draw()", adiciona no fim da funçao o seguinte:

for index, inimigo in ipairs(inimigos) do
   love.graphics.draw(inimigo.img, inimigo.posicaoX, inimigo.posicaoY)
end

3. No fim do teu codigo adiciona esta função:

function atualizarInimigos(dt)
 geraInimigoTempo = geraInimigoTempo - dt
 if geraInimigoTempo <= 0 then
  geraInimigoTempo = geraInimigoTempoMax
  y = love.math.random(0, love.graphics.getHeight() - 64)
  inimigo = {posicaoX = love.graphics.getWidth(), posicaoY = y, width = 64, height = 64, velocidade = 100, img = ImagemInimigo}
  table.insert(inimigos, inimigo)
 end
 for i = #inimigos, 1, -1 do
  inimigo = inimigos[i]
  inimigo.posicaoX = inimigo.posicaoX - inimigo.velocidade * dt
 end
end

4. Por fim na função "update(dt)" depois da linha "atualizarMisseis(dt)" adiciona esta linha:

   atualizarInimigos(dt) 

Agora Deves ter um monte de inimigos a vir em direção da tua nave. Altera o valor de "geraInimigoTempoMax" para teres á tua escolha a quantidade de inimigos que vai aparecendo ao longo do tempo.

    

Agora temos tudo sobre o jogador feito, passaremos para os inimigos.

Vamos começar por indicar as imagens as suas variaveis na função "load()" (podes mudar a imagem):

<span style="color:cyan">
ImagemInimigo = love.graphics.newImage("inimigo1.png")
inimigos = {}
geraInimigoTempo = 0
geraInimigoTempoMax = 0.5
</span>
Agora vamos editar na função "draw()", adiciona no fim da função o seguinte:
<span style="color:cyan">for index, inimigo in ipairs(inimigos) do
        love.graphics.draw(inimigo.img, inimigo.posicaoX, inimigo.posicaoY)
    end</span>

No fim do teu código adiciona esta função:

<span style="color:purple">
function atualizarInimigos(dt)
<span style="color:cyan">geraInimigoTempo = geraInimigoTempo - dt
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

</span>


<span style="color:white">Esta função faz com que o inimigo apareça de certo em certo tempo.</span>

Por fim na função "update(dt)" depois da linha "atualizarMisseis(dt)" adiciona esta linha:
<span style="color:cyan">atualizarInimigos(dt) 

Agora deves ter um monte de inimigos a vir em direção da tua nave(alt+L). Altera o valor de "geraInimigoTempoMax" para teres à tua escolha a quantidade de inimigos que vai aparecendo ao longo do tempo.

    
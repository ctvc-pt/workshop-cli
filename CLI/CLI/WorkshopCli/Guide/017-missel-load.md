[color=white]
O próximo passo é meter a nossa nave a disparar mísseis e, tal 
como fizemos para adicionar a nave ao jogo, o primeiro passo é 
carregar e desenhar a imagem.

1. Primeiro escolhe dentro da pasta "mygame" uma imagem para ser 
o míssil da tua nave. 

Adiciona o seguinte código na função "load()" antes da palavra "end":
[/color] [color=blue]
    missilImagem = love.graphics.newImage("missil1.png")
    misseis = {}
    podeDisparar = true
[/color] [color=white]
Aqui:
- "missilImagem" guarda a imagem que vais usar em cada míssil.
- "misseis = {}" cria uma lista vazia. De cada vez que disparas, 
  um novo míssil é adicionado a esta lista.
- "podeDisparar" é uma flag (verdadeiro/falso) que indica se a 
  nave já pode disparar o próximo míssil.

2. Agora vamos desenhar os mísseis na função "draw()"
   [/color] [color=blue]
    for index, missil in ipairs(misseis) do
       love.graphics.draw(missil.imagem, missil.posicaoX, missil.posicaoY)
    end
   [/color] [color=white]
3. Insere na função update(dt) o seguinte código:
   [/color] [color=blue]
    if love.keyboard.isDown("space") then
      if podeDisparar then
        missil = {
           posicaoX = posicaoX + 64, 
           posicaoY = posicaoY + 32,
           width = 16, height=16,
           velocidade = missilVelocidade, 
           imagem = missilImagem 
        }
        table.insert(misseis, missil)
      end
    end
   [/color] [color=white]
Como funciona:
- "love.keyboard.isDown('space')" é verdadeiro enquanto a tecla 
  ESPAÇO estiver pressionada.
- "table.insert(misseis, missil)" adiciona o novo míssil à lista.
- O ciclo "for" na função "draw()" percorre todos os mísseis da 
  lista e desenha cada um na sua posição.

4. Inicia o jogo e vê o que há de novo.

Os mísseis não saem do sítio, certo? É isso que vamos fazer a seguir.
[/color]
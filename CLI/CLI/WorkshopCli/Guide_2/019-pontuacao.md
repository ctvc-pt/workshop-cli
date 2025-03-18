[color=white]
Agora vamos adicionar a pontuação ao jogo e um som que será tocado todas 
as vezes que o jogador passar por um cano.

1. Dentro da função "load()" e na função "reiniciarJogo()", coloca a seguinte linha: 
   [/color] [color=blue]
   pontuacao = 0  
   [/color] [color=white]

2. Dentro da função "update(dt)" adiciona o seguinte código:
   [/color] [color=blue]
   for i, cano in ipairs(canos) do
      if cano.x + larguraCano < 0 then
         table.remove(canos, i)
         pontuacao = pontuacao + 1
         somPontuacao:play()
      end
   end
   [/color] [color=white]
Este código vai fazer com que, sempre um cano sair da tela do lado esquerdo, ele 
vai aumentar a pontuação em 1.

3. Para ver a pontuação vamos colocar na função "draw()" o seguinte:
   [/color] [color=blue]
   love.graphics.setFont(fonte)
   love.graphics.setColor(1, 1, 1)
   love.graphics.printf("Score: " .. pontuacao, 10, 10, love.graphics.getWidth(), "left")
   [/color] [color=white]
4. Agora vamos adicionar na função "load()" a linha que vai carregar o som da pontuação:
   [/color] [color=blue]
   somPontuacao = love.audio.newSource("point.wav", "static")
   [/color] [color=white]
5. De seguida vamos fazer com que os textos do jogo fiquem mais bonitos, adiciona na função 
"load()" a seguinte linha:
   [/color] [color=blue]
   fonte = love.graphics.newFont(30)  
   [/color] [color=white]
Se quiseres modificar o tamanho da fonte, é só substituires o número "30" pelo numero que 
quiseres.

[/color] 
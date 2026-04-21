
-+
[color=white]
Para dar mais dificuldade vamos criar os obstáculos do jogo.

1. Dentro da função "load()" adiciona o seguinte código:
[/color] [color=blue]
   canos = {}
   larguraCano = 50
   espacoCano = 150
   velocidadeCano = 2
   tempoGerarCano = 2
   temporizadorCanos = 0
[/color] [color=white]

Aqui definimos a largura dos canos, (larguraCano = 50), o espaço 
entre o cano superior e o inferior (espacoCano = 150) e a velocidade (velocidadeCano = 2).

"canos = {}" cria uma lista vazia. Como vão aparecer vários canos 
ao longo do jogo, guardamo-los todos numa lista e vamos 
adicionando novos à medida que o tempo passa.

2. Dentro da função "update(dt)", adiciona o que faz os canos moverem-se para a esquerda:
[/color] [color=blue]
   for i, cano in ipairs(canos) do
       cano.x = cano.x - velocidadeCano
   end
   temporizadorCanos = temporizadorCanos + dt
   if temporizadorCanos >= tempoGerarCano then
       local alturaCano = love.math.random(50, love.graphics.getHeight() - espacoCano - 50)
       table.insert(canos, { x = love.graphics.getWidth(), y = alturaCano })
       temporizadorCanos = 0
   end
[/color] [color=white]

3. Dentro da função "draw()" adiciona:
[/color] [color=blue]
   love.graphics.setColor(0, 0.5, 0)
   for i, cano in ipairs(canos) do
       love.graphics.rectangle("fill", cano.x, 0, larguraCano, cano.y)
       love.graphics.rectangle(
           "fill",
           cano.x,
           cano.y + espacoCano,
           larguraCano,
           love.graphics.getHeight() - cano.y - espacoCano
       )
   end
   love.graphics.setColor(1, 1, 1)

[/color] [color=white]

Como funciona:
- "for i, cano in ipairs(canos)" percorre todos os canos da lista. 
  "ipairs" é uma função do Lua que serve para atravessar listas.
- "cano.x = cano.x - velocidadeCano" faz cada cano mover-se para 
  a esquerda em cada frame.
- "temporizadorCanos" soma "dt" a cada frame. Quando passa o 
  "tempoGerarCano" (2 segundos), criamos um novo par de canos 
  com altura aleatória e pomos o contador a zero.
- "table.insert(canos, { x = ..., y = ... })" adiciona um novo 
  cano ao fim da lista.

Agora os canos já vão aparecer dentro do jogo, e também vão andar para o lado 
esquerdo do ecrã. Executa o jogo com "Alt + L", e diverte-te um pouco.

[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Desfio-te a aumentares a largura do cano.
[/color] 
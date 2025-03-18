[color=white]
O pássaro precisa de bater as asas! Vamos permitir que ele 
suba quando pressionarmos a tecla "espaço".

4. Dentro da função "update()" adiciona o seguinte código:
[/color] [color=blue]
   if love.keyboard.isDown("space") then
        passaro.velocidade = passaro.salto
   end
[/color] [color=white]

Agora, quando pressionares espaço, o pássaro sobe!
Executa o jogo (Alt + L), Experimenta carregar espaço várias 
vezes e vê como o pássaro sobe e desce.
[/color] [color=red]
     ____  _____ ____    _    _____ ___ ___  
    |  _ \| ____/ ___|  / \  |  ___|_ _/ _ \
    | | | |  _| \___ \ / _ \ | |_   | | | | |
    | |_| | |___ ___) / ___ \|  _|  | | |_| |
    |____/|_____|____/_/   \_\_|   |___\___/
[/color] [color=white]

Muda o valor do salto do pássaro para -6.

[/color] 
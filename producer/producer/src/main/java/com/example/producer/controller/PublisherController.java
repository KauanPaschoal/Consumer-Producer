package com.example.producer.controller;

import com.example.producer.config.RabbitConfig;
import com.example.producer.dto.MessageDto;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/publish")
public class PublisherController {
    private final RabbitTemplate rabbitTemplate;

    public PublisherController(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    @PostMapping
    public ResponseEntity<?> publish(@RequestBody MessageDto message) {
        rabbitTemplate.convertAndSend(RabbitConfig.QUEUE_NAME, message);
        return ResponseEntity.status(201).body(
                String.format("Mensagem publicada (id=%s) na fila %s", message.getId(), RabbitConfig.QUEUE_NAME)
        );
    }
}

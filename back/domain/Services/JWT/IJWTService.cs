using stock.domain.entities;
namespace stock.domain.services;

public interface IJWTService {
    public string generate(User user);
}
var Bidder = {};

// Models

Bidder.chatMessage = function (sender, content, dateSent) {
    var self = this;
    self.username = sender;
    self.content = content;
    if (dateSent != null) {
        self.timestamp = dateSent;
    }
}

Bidder.user = function (username, userId) {
    var self = this;
    self.username = username;
    self.id = userId;
}

Bidder.bid= function(username, bidPrice) {
    var self = this;
    self.username = username;
    self.price = bidPrice;
}

// ViewModels

Bidder.chatViewModel = function () {
    var self = this;
    self.messages = ko.observableArray();
}

Bidder.connectedUsersViewModel = function () {
    var self = this;
    self.contacts = ko.observableArray();
    self.customRemove = function (userToRemove) {
        var userIdToRemove = userToRemove.id;
        self.contacts.remove(function (item) {
            return item.id === userIdToRemove;
        });
    }
}


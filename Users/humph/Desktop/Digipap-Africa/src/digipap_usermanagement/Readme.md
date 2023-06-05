# Digipap
This project is for the Alpha Tracker
Project Divisions
  - Backend
  - Mobile App - React Native
  - Web App - TypeScript
  - Documentation
  
 Backend Proj Structure
  - Use Repository Pattern
  - Models
  - IRepositories
  - Repositories
  - Persistent
  - Controllers
  - ReadMe
  - Project.cs
  - DockerFile
  
  1.0 Models - Fields marked * are compulsary
  
     - UserEntity 
      - UserId - PrimaryKey
      - UserName *
      - Email *
      - PhoneNumber *
      - Password *
      - ConfirmPassword
      - IsActive
      - IsDeleted
      - IsApproved
      - CreatedOn
      - CreatedBy
      - UpdatedOn
      - UpdatedBy
      - ProfilePictureUrl
      - NationalId * 
      - Collection<UserReferals> Tractors Nullable  
         
  
